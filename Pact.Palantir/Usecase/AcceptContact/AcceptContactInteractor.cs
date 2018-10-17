﻿namespace Pact.Palantir.Usecase.AcceptContact
{
  using System;
  using System.Threading.Tasks;

  using Pact.Palantir.Encryption;
  using Pact.Palantir.Entity;
  using Pact.Palantir.Exception;
  using Pact.Palantir.Repository;
  using Pact.Palantir.Service;

  /// <summary>
  /// The accept contact interactor.
  /// </summary>
  public class AcceptContactInteractor : AbstractContactInteractor<AcceptContactRequest, AcceptContactResponse>
  {
    /// <inheritdoc />
    public AcceptContactInteractor(IContactRepository repository, IMessenger messenger, IEncryption encryption)
      : base(repository, messenger, encryption)
    {
    }

    /// <inheritdoc />
    public override async Task<AcceptContactResponse> ExecuteAsync(AcceptContactRequest request)
    {
      try
      {
        var contactDetails = new Contact
                               {
                                 Name = request.UserName,
                                 ImagePath = request.UserImagePath,
                                 ChatAddress = request.ChatAddress.Value,
                                 ChatKeyAddress = request.ChatKeyAddress.Value,
                                 ContactAddress = request.UserContactAddress.Value,
                                 PublicKeyAddress = request.UserPublicKeyAddress.Value,
                                 Rejected = false,
                                 Request = false,
                                 PublicKey = null
                               };

        // Generate chat pass salt here so we exit the interactor when it fails, before sending something
        var chatPasSalt = await this.GetChatPasswordSalt(request.ChatKeyAddress, request.UserKeyPair);

        var contactInformation = await this.Repository.LoadContactInformationByAddressAsync(request.ContactPublicKeyAddress);
        var contactExchange = ContactExchange.Create(contactDetails, contactInformation.PublicKey, request.UserKeyPair.PublicKey);

        await this.SendContactDetails(contactExchange.Payload, contactInformation);
        await this.ExchangeKey(contactDetails, contactInformation.PublicKey, chatPasSalt);

        await this.Repository.AddContactAsync(request.ChatAddress.Value, true, contactDetails.PublicKeyAddress);
        return new AcceptContactResponse { Code = ResponseCode.Success };
      }
      catch (MessengerException exception)
      {
        return new AcceptContactResponse { Code = exception.Code };
      }
      catch (Exception)
      {
        return new AcceptContactResponse { Code = ResponseCode.UnkownException };
      }
    }
  }
}