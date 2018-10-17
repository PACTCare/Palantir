﻿namespace Pact.Palantir.Tests.Usecase
{
  using System.Linq;
  using System.Threading.Tasks;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Pact.Palantir.Tests.Encryption;
  using Pact.Palantir.Tests.Repository;
  using Pact.Palantir.Tests.Service;
  using Pact.Palantir.Usecase;
  using Pact.Palantir.Usecase.SendMessage;

  using Tangle.Net.Entity;

  /// <summary>
  /// The send message interactor test.
  /// </summary>
  [TestClass]
  public class SendMessageInteractorTest
  {
    /// <summary>
    /// The test message is too long should return error code.
    /// </summary>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    [TestMethod]
    public async Task TestMessageIsTooLongShouldReturnErrorCode()
    {
      var interactor = new SendMessageInteractor(new InMemoryMessenger(), new EncryptionStub(), new EncryptionStub());
      var response = await interactor.ExecuteAsync(
                       new SendMessageRequest { Message = new string(Enumerable.Repeat('a', Constants.MessageCharacterLimit + 1).ToArray()) });

      Assert.AreEqual(ResponseCode.MessageTooLong, response.Code);
    }

    [TestMethod]
    public async Task TestMessengerThrowsExceptionShouldReturnErrorCode()
    {
      var interactor = new SendMessageInteractor(new ExceptionMessenger(), new EncryptionStub(), new EncryptionStub());
      var response = await interactor.ExecuteAsync(
                       new SendMessageRequest
                         {
                           Message = new string(Enumerable.Repeat('a', Constants.MessageCharacterLimit).ToArray()),
                           ChatAddress = new Address(Hash.Empty.Value),
                           ChatKeyPair = InMemoryContactRepository.NtruKeyPair,
                           UserPublicKeyAddress = new Address(Hash.Empty.Value)
                       });

      Assert.AreEqual(ResponseCode.MessengerException, response.Code);
    }
  }
}