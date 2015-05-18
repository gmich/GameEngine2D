﻿using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using Gem.Gui.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Gem.Gui.Alignment;

namespace Gem.Gui.Controls
{
    public class Label : AControl
    {
        public Label(string text,
                     SpriteFont font,
                     Texture2D texture,
                     Region region,
                     Color textcolor,
                     AlignmentContext alignmentContext)
            : base(texture, region, new NoStyle())
        {
            this.Text = new StandardText(font, Vector2.Zero, text, alignmentContext);
            this.Options.IsEnabled = false;
        }
    }
}