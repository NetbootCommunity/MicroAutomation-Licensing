﻿using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace MicroAutomation.Licensing.Security.Cryptography;

/// <summary>
/// Represents a generator for signature keys of <see cref="License"/>.
/// </summary>
public class KeyGenerator
{
    private readonly IAsymmetricCipherKeyPairGenerator keyGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyGenerator"/> class
    /// with a key size of 256 bits.
    /// </summary>
    public KeyGenerator() : this(256)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyGenerator"/> class
    /// with the specified key size.
    /// </summary>
    /// <remarks>Following key sizes are supported:
    /// - 192
    /// - 224
    /// - 239
    /// - 256 (default)
    /// - 384
    /// - 521</remarks>
    /// <param name="keySize">The key size.</param>
    public KeyGenerator(int keySize) : this(keySize, SecureRandom.GetInstance("SHA256PRNG").GenerateSeed(32))
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyGenerator"/> class
    /// with the specified key size and seed.
    /// </summary>
    /// <remarks>Following key sizes are supported:
    /// - 192
    /// - 224
    /// - 239
    /// - 256 (default)
    /// - 384
    /// - 521</remarks>
    /// <param name="keySize">The key size.</param>
    /// <param name="seed">The seed.</param>
    public KeyGenerator(int keySize, byte[] seed)
    {
        var secureRandom = SecureRandom.GetInstance("SHA256PRNG");
        secureRandom.SetSeed(seed);

        var keyParams = new KeyGenerationParameters(secureRandom, keySize);
        keyGenerator = new ECKeyPairGenerator();
        keyGenerator.Init(keyParams);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="KeyGenerator"/> class.
    /// </summary>
    public static KeyGenerator Create()
        => new();

    /// <summary>
    /// Generates a private/public key pair for license signing.
    /// </summary>
    /// <returns>An <see cref="KeyPair"/> containing the keys.</returns>
    public KeyPair GenerateKeyPair()
        => new(keyGenerator.GenerateKeyPair());
}