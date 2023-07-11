
namespace Hydra.FileStorage.Infrastructure.SignatureVerify.FileTypes
{
    public sealed class Xls : FileType
    {
        public Xls()
        {
            Name = "XLS";
            Description = "XLS MICROSOFT OFFICE DOCUMENT";
            AddExtensions("xls");
            AddSignatures(
                new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 },
                new byte[] { 0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00 },
                new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x10 },
                new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x1F },
                new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x22 },
                new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x23 },
                new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x28 },
                new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x29 }
            );
        }
    }
}
