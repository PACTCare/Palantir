﻿namespace Pact.Palantir.Tests.Repository
{
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;
  using System.Threading.Tasks;

  using Pact.Palantir.Encryption;
  using Pact.Palantir.Entity;
  using Pact.Palantir.Repository;

  using Tangle.Net.Entity;

  using VTDev.Libraries.CEXEngine.Crypto.Cipher.Asymmetric.Interfaces;

  /// <summary>
  /// The in memory contact repository.
  /// </summary>
  [ExcludeFromCodeCoverage]
  internal class InMemoryContactRepository : IContactRepository
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryContactRepository"/> class.
    /// </summary>
    public InMemoryContactRepository()
    {
      this.PersistedContacts = new List<Contact>();
    }

    /// <summary>
    /// Gets the persisted contacts.
    /// </summary>
    public List<Contact> PersistedContacts { get; }

    /// <inheritdoc />
    public async Task AddContactAsync(string address, bool accepted, string publicKeyAddress)
    {
      this.PersistedContacts.Add(new Contact { ChatAddress = address, Rejected = !accepted, PublicKeyAddress = publicKeyAddress });
    }

    /// <inheritdoc />
    public async Task<ContactInformation> LoadContactInformationByAddressAsync(Address address)
    {
      return new ContactInformation { ContactAddress = address, PublicKey = NtruKeyPair.PublicKey };
    }

    /// <inheritdoc />
    public async Task<List<Contact>> LoadContactsAsync(string publicKeyAddress)
    {
      return this.PersistedContacts.Where(c => c.PublicKeyAddress == publicKeyAddress && !c.Rejected).ToList();
    }

    private static IAsymmetricKeyPair ntruKeyPair;

    internal static IAsymmetricKeyPair NtruKeyPair =>
      ntruKeyPair ?? (ntruKeyPair = NtruEncryption.Key.CreateAsymmetricKeyPair(Seed.Random().Value.ToLower(), Seed.Random().Value.ToLower()));
  }
}