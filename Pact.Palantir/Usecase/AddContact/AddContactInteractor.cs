﻿namespace Pact.Palantir.Usecase.AddContact
{
  using System;
  using System.Threading.Tasks;

  using Pact.Palantir.Entity;
  using Pact.Palantir.Exception;
  using Pact.Palantir.Repository;
  using Pact.Palantir.Service;

  using Tangle.Net.Entity;

  /// <inheritdoc />
  public class AddContactInteractor : AbstractContactInteractor<AddContactRequest, AddContactResponse>
  {
    /// <inheritdoc />
    public AddContactInteractor(IContactRepository repository, IMessenger messenger)
      : base(repository, messenger, null)
    {
    }

    /// <inheritdoc />
    public override async Task<AddContactResponse> ExecuteAsync(AddContactRequest request)
    {
      try
      {
        var requesterDetails = new Contact
                                 {
                                   ChatAddress = Seed.Random().ToString(),
                                   ChatKeyAddress = Seed.Random().ToString(),
                                   Name = request.Name,
                                   ImagePath = request.ImagePath,
                                   ContactAddress = request.RequestAddress.Value,
                                   Request = true,
                                   Rejected = false,
                                   PublicKey = null,
                                   PublicKeyAddress = request.PublicKeyAddress.Value
                                 };

        var contactInformation = await this.Repository.LoadContactInformationByAddressAsync(request.ContactAddress);
        var contactExchange = ContactExchange.Create(requesterDetails, contactInformation.PublicKey, request.UserPublicKey);

        await this.SendContactDetails(contactExchange.Payload, contactInformation);
        await this.ExchangeKey(requesterDetails, contactInformation.PublicKey, GetChatPasSalt());

        await this.Repository.AddContactAsync(requesterDetails.ChatAddress, true, requesterDetails.PublicKeyAddress);

        return new AddContactResponse { Code = ResponseCode.Success };
      }
      catch (MessengerException exception)
      {
        return new AddContactResponse { Code = exception.Code };
      }
      catch (Exception)
      {
        return new AddContactResponse { Code = ResponseCode.UnkownException };
      }
    }

    /// <summary>
    /// The get chat pas salt.
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    private static string GetChatPasSalt()
    {
      return Seed.Random() + Seed.Random().ToString().Substring(0, 20);
    }
  }
}