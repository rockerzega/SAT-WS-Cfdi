using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Oiw;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Security;

/// <summary>
///     En Linux con OpenSSL 3 (p. ej. Fedora/RHEL), la política criptográfica puede rechazar
///     <see cref="RSA.SignHash" /> con SHA1. SignedXml usa ese camino para XmlDsig rsa-sha1.
///     Esta clase reenvía solo SHA1+PKCS#1 a BouncyCastle (RSA puro), manteniendo el algoritmo esperado por el SAT.
/// </summary>
internal sealed class RsaSha1Pkcs1BouncyCastle : RSA
{
    private readonly RSA _inner;

    public RsaSha1Pkcs1BouncyCastle(RSA inner)
    {
        _inner = inner;
    }

    public override int KeySize => _inner.KeySize;

    public override RSAParameters ExportParameters(bool includePrivateParameters) =>
        _inner.ExportParameters(includePrivateParameters);

    public override void ImportParameters(RSAParameters parameters) =>
        _inner.ImportParameters(parameters);

    public override byte[] SignHash(byte[] hash, HashAlgorithmName hashAlgorithm, RSASignaturePadding padding)
    {
        if (hashAlgorithm == HashAlgorithmName.SHA1 && padding == RSASignaturePadding.Pkcs1)
            return SignPkcs1DigestInfoSha1(hash);

        return _inner.SignHash(hash, hashAlgorithm, padding);
    }

    public override bool TrySignHash(ReadOnlySpan<byte> hash, Span<byte> destination,
        HashAlgorithmName hashAlgorithm, RSASignaturePadding padding, out int bytesWritten)
    {
        if (hashAlgorithm == HashAlgorithmName.SHA1 && padding == RSASignaturePadding.Pkcs1)
        {
            var sig = SignPkcs1DigestInfoSha1(hash.ToArray());
            if (destination.Length < sig.Length)
            {
                bytesWritten = 0;
                return false;
            }

            sig.CopyTo(destination);
            bytesWritten = sig.Length;
            return true;
        }

        return _inner.TrySignHash(hash, destination, hashAlgorithm, padding, out bytesWritten);
    }

    public override byte[] Decrypt(byte[] data, RSAEncryptionPadding padding) =>
        _inner.Decrypt(data, padding);

    public override byte[] Encrypt(byte[] data, RSAEncryptionPadding padding) =>
        _inner.Encrypt(data, padding);

    public override bool VerifyHash(byte[] hash, byte[] signature, HashAlgorithmName hashAlgorithm,
        RSASignaturePadding padding) =>
        _inner.VerifyHash(hash, signature, hashAlgorithm, padding);

    public override bool TryDecrypt(ReadOnlySpan<byte> data, Span<byte> destination,
        RSAEncryptionPadding padding, out int bytesWritten) =>
        _inner.TryDecrypt(data, destination, padding, out bytesWritten);

    public override bool TryEncrypt(ReadOnlySpan<byte> data, Span<byte> destination,
        RSAEncryptionPadding padding, out int bytesWritten) =>
        _inner.TryEncrypt(data, destination, padding, out bytesWritten);

    private byte[] SignPkcs1DigestInfoSha1(byte[] sha1Hash20)
    {
        ArgumentNullException.ThrowIfNull(sha1Hash20);
        if (sha1Hash20.Length != 20)
            throw new CryptographicException($"Se esperaba hash SHA1 de 20 bytes, longitud={sha1Hash20.Length}.");

        var priv = ToPrivateCrtParameters(_inner);
        var digestInfo = new DigestInfo(
            new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1, DerNull.Instance),
            sha1Hash20);
        var block = digestInfo.GetDerEncoded();

        var engine = new Pkcs1Encoding(new RsaBlindedEngine());
        engine.Init(true, priv);
        return engine.ProcessBlock(block, 0, block.Length);
    }

    private static RsaPrivateCrtKeyParameters ToPrivateCrtParameters(RSA rsa)
    {
        var p = rsa.ExportParameters(true);
        return new RsaPrivateCrtKeyParameters(
            new BigInteger(1, p.Modulus!),
            new BigInteger(1, p.Exponent!),
            new BigInteger(1, p.D!),
            new BigInteger(1, p.P!),
            new BigInteger(1, p.Q!),
            new BigInteger(1, p.DP!),
            new BigInteger(1, p.DQ!),
            new BigInteger(1, p.InverseQ!));
    }
}
