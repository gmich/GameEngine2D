﻿using Gem.Gui.Aggregation;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Elements.Areas
{
    public interface IGuiArea : IGuiComponent
    {
        event EventHandler<ElementEventArgs> OnAddComponent;
        event EventHandler<ElementEventArgs> OnRemoveComponent;

        int ComponentCount { get; }

        int AddComponent(IGuiComponent component);

        bool RemoveComponent(int id);

        IEnumerable<GuiAreaEntry> Entries();

        ComponentIndex FocusIndex { get; set; }

        IGuiComponent this[int id] { get; }
    }


}