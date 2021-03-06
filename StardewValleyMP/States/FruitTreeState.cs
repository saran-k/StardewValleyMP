﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.TerrainFeatures;

namespace StardewValleyMP.States
{
    public class FruitTreeState : State
    {
        public bool stump;
        public int fruitsOnTree;

        public FruitTreeState(FruitTree tree)
        {
            stump = tree.stump;
            fruitsOnTree = tree.fruitsOnTree;
        }

        public override bool isDifferentEnoughFromOldStateToSend(State obj)
        {
            FruitTreeState tree = obj as FruitTreeState;
            if (tree == null) return false;

            if (tree.stump != stump) return true;
            if (tree.fruitsOnTree != fruitsOnTree) return true;

            return false;
        }
    }
}
