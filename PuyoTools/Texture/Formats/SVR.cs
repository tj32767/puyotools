﻿using System;
using System.IO;
using System.Drawing;
using VrSharp.SvrTexture;

namespace PuyoTools2.Texture
{
    public class SVR : TextureBase
    {
        public override void Read(byte[] source, long offset, out Bitmap destination, int length)
        {
            // Some SVR textures require an external clut, so we'll just pass this off to ReadWithCLUT
            ReadWithPalette(source, offset, null, 0, out destination, length, 0);
        }

        public override void ReadWithPalette(byte[] source, long offset, byte[] palette, long paletteOffset, out Bitmap destination, int length, int paletteLength)
        {
            // Reading SVR textures is done through VrSharp, so just pass it to that
            SvrTexture texture = new SvrTexture(source, offset, length);

            // Check to see if this texture requires an external palette.
            // If it does and none was set, throw an exception.
            if (texture.NeedsExternalClut())
            {
                if (palette != null && paletteLength > 0)
                    texture.SetClut(new SvpClut(palette, paletteOffset, paletteLength));
                else
                    throw new TextureNeedsPalette();
            }

            destination = texture.GetTextureAsBitmap();
        }

        public override void Write(byte[] source, long offset, Stream destination, int length, string fname)
        {
            throw new NotImplementedException();
        }

        public override bool Is(Stream source, int length, string fname)
        {
            return (length > 16 && SvrTexture.IsSvrTexture(source, length));
        }

        public override bool CanWrite()
        {
            throw new NotImplementedException();
        }
    }
}