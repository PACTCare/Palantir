## Add Contact

Assuming a user (Sender) wants to interact with another (Receiver), he/she may want to send a contact request. Simply input the senders information along with the receivers request address (as contact address).

### Request
```csharp
public class AddContactRequest
{
    /// <summary>
    /// Public key address of the contact that should be added
    /// </summary>
    public Address ContactAddress { get; set; }

    /// <summary>
    /// Optional: Image that will be shown to the added contact within the contact request
    /// </summary>
    public string ImagePath { get; set; }

    /// <summary>
    /// Current user name. Will be shown to the contact within the contact request
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Public key address of the current user
    /// </summary>
    public Address PublicKeyAddress { get; set; }

    /// <summary>
    /// Request address of the current user
    /// </summary>
    public Address RequestAddress { get; set; }

    /// <summary>
    /// Current user public key. Used to encrypt the nonce of the answer to the contact request
    /// </summary>
    public IAsymmetricKey UserPublicKey { get; set; }
}
```

### Usage
```csharp
var response = await this.AddContactInteractor.ExecuteAsync(
        new AddContactRequest
        {
            Name = UserService.CurrentUser.Name,
            ImagePath = UserService.CurrentUser.ImageHash,
            RequestAddress = new Address(UserService.CurrentUser.RequestAddress),
            PublicKeyAddress = new Address(UserService.CurrentUser.PublicKeyAddress),
            ContactAddress = new Address(this.RequestAddress)
        });
```