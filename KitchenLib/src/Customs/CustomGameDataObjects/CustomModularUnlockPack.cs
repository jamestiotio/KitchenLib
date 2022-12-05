﻿using KitchenData;
using System.Collections.Generic;

namespace KitchenLib.Customs
{
	public abstract class CustomModularUnlockPack : CustomUnlockPack
	{

		public virtual List<IUnlockSet> Sets { get { return new List<IUnlockSet>(); } }
		public virtual List<IUnlockFilter> Filter { get { return new List<IUnlockFilter>(); } }
		public virtual List<IUnlockSorter> Sorters { get { return new List<IUnlockSorter>(); } }
		public virtual List<ConditionalOptions> ConditionalOptions { get { return new List<ConditionalOptions>(); } }
		
		public override void Convert(GameData gameData, out GameDataObject gameDataObject)
		{
			ModularUnlockPack result = new ModularUnlockPack();
			ModularUnlockPack empty = new ModularUnlockPack();

			if (empty.ID != ID) result.ID = ID;
			if (empty.Sets != Sets) result.Sets = Sets;
			if (empty.Filter != Filter) result.Filter = Filter;
			if (empty.Sorters != Sorters) result.Sorters = Sorters;
			if (empty.ConditionalOptions != ConditionalOptions) result.ConditionalOptions = ConditionalOptions;

			gameDataObject = result;
		}
	}
}
