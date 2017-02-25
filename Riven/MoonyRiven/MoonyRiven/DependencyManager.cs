using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using MoonyRiven.Properties;
using SharpDX;
using Color = System.Drawing.Color;
using Sprite = EloBuddy.SDK.Rendering.Sprite;

namespace MoonyRiven
{
    public class DependencyManager
    {
        public class AnchorPiece
        {
            public AnchorPiece(CastType castType, int level, bool isAnchor)
            {
                CastType = castType;
                Level = level;
                IsAnchor = isAnchor;
            }

            public int Level { get; }
            public bool IsAnchor { get; }
            public CastType CastType { get; }
            public Vector2 Position { get; set; }
        }

        public class Anchor
        {
            public AnchorPiece RealAnchor { get; }
            public bool IsBest { get; set; }

            public List<AnchorPiece> Subs  = new List<AnchorPiece>();

            public Anchor(AnchorPiece realAnchor)
            {
                RealAnchor = realAnchor;
            }

            public int GetSubPriority(CastType t)
            {
                CastType type = RealAnchor.CastType;
                string s = $"Advanced_{type}_{t}";
                return RivenMenu.Combo[s].Cast<Slider>().CurrentValue;
            }

            public List<CastType> GetOnlyChildTypes(out CastType bestType)
            {
                bestType = CastType.NONE;

                CastType type = RealAnchor.CastType;
                string s = $"Advanced_{type}_";
                List<CastType> castTypes = new List<CastType>();
                float maxPrio = 0;

                foreach (CastType castType in Enum.GetValues(typeof(CastType)))
                {
                    if (castType == CastType.NONE)
                        continue;

                    string searched = s + castType;
                    var valueBase = RivenMenu.Combo[searched];
                    if (valueBase != null) //combo exists
                    {
                        int prio = valueBase.Cast<Slider>().CurrentValue;
                        bool castContainsR = castType == CastType.R2 || type == CastType.R2 || castType == CastType.R1 ||
                                             castType == CastType.R1;
                        bool maxDependency = RivenMenu.Draw["onlyMaxPrioDependency"].Cast<CheckBox>().CurrentValue;
                        if (!maxDependency)
                        {
                            if (prio >= 1 && !castContainsR)
                            {
                                castTypes.Insert(0, castType);

                                if (prio > maxPrio)
                                {
                                    maxPrio = prio;
                                    bestType = castType;
                                }
                            }
                        }
                        else if (prio >= maxPrio && maxDependency)
                        {
                            //Chat.Print(searched);
                            maxPrio = prio;
                            castTypes.Clear();
                            castTypes.Insert(0, castType);
                        }
                    }
                }
                
                return castTypes;
            }
        }

        private Dictionary<CastType, Sprite> Images = new Dictionary<CastType, Sprite>(); 
        public DependencyManager()
        {
            Images.Add(CastType.AA, new Sprite(TextureLoader.BitmapToTexture(Resources.AA)));
            Images.Add(CastType.Q, new Sprite(TextureLoader.BitmapToTexture(Resources.Q)));
            Images.Add(CastType.W, new Sprite(TextureLoader.BitmapToTexture(Resources.W)));
            Images.Add(CastType.E, new Sprite(TextureLoader.BitmapToTexture(Resources.E)));
            Images.Add(CastType.R1, new Sprite(TextureLoader.BitmapToTexture(Resources.R1)));
            Images.Add(CastType.R2, new Sprite(TextureLoader.BitmapToTexture(Resources.R2)));
            Images.Add(CastType.F, new Sprite(TextureLoader.BitmapToTexture(Resources.F)));
            Images.Add(CastType.H, new Sprite(TextureLoader.BitmapToTexture(Resources.H)));
            Images.Add(CastType.NONE, new Sprite(TextureLoader.BitmapToTexture(Resources.Arrow)));


            //foreach (var treeElement in GetTree())
            //{
            //    treeElement.
            //}

            Drawing.OnDraw += DrawingOnOnDraw;
        }

        /// <summary>
        /// Linear slope
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public float GetXWidth(int lvl)
        {
            //a = -0.600703
            //lvl 4 => 100 width
            //return (float)(Drawing.Width*0.4*Math.Pow(-0.600703f, lvl));
            switch (lvl)
            {
                case 1:
                    return Drawing.Width * .6f;
                case 2:
                    return Drawing.Width*.2f;
                case 3:
                    return Drawing.Width * 0.08f;
                case 4:
                    return Drawing.Width * 0.05f;
                case 5:
                    return Drawing.Width * .025f;
            }
            return 0;
        }
        /// <summary>
        /// Each TreeElement represents a branch with all childs
        /// </summary>
        /// <returns></returns>
        public List<Anchor> GetTree()
        {
            string root = RivenMenu.Draw["dependencyRoot"].Cast<ComboBox>().SelectedText;
            CastType rootType = (CastType) Enum.Parse(typeof (CastType), root);
            List<Anchor> treeElements = new List<Anchor>();

            var first = new Anchor(new AnchorPiece(rootType, 1, true)) {RealAnchor = { Position = new Vector2(Drawing.Width*0.5f, 50)}};
            treeElements.Add(first);

            for (int level = 1; level <= 10; level++)
            {
                var adds = new List<Anchor>();
                // ReSharper disable once AccessToModifiedClosure
                foreach (var anchor in treeElements.Where(e => e.RealAnchor.Level == level))
                {
                    /*enumerate through sub types*/
                    CastType bestSub;
                    var subs = anchor.GetOnlyChildTypes(out bestSub);
                    float maxX = GetXWidth(level);
                    float XPos = anchor.RealAnchor.Position.X - maxX/2;
                    float YPos = anchor.RealAnchor.Position.Y + 200;

                    for (int subIndex = 0; subIndex < subs.Count; subIndex++)
                    {
                        CastType treeSubCastType = subs[subIndex];
                        /*already in path contained*/
                        if (treeElements.Where(e => e.RealAnchor.Level < level).Any(anchorr => anchorr.RealAnchor.CastType == treeSubCastType))
                        {
                            continue;
                        }

                        Vector2 subPos = new Vector2(XPos, YPos);
                        if (subPos.X > Drawing.Width)
                            subPos.X = Drawing.Width - 64;

                        anchor.Subs.Add(new AnchorPiece(treeSubCastType, anchor.RealAnchor.Level+1, false) {Position = subPos});

                        var nextAnchor = new Anchor(new AnchorPiece(treeSubCastType, anchor.RealAnchor.Level + 1, true))
                            { IsBest = treeSubCastType == bestSub};
                        CastType t;
                        if (nextAnchor.GetOnlyChildTypes(out t).Any())
                        {
                            nextAnchor.RealAnchor.Position = subPos;
                            adds.Add(nextAnchor);

                        }

                        XPos += maxX / (subs.Count - 1)*(subIndex+1);
                    }
                }

                foreach (var anchor in adds)
                {
                    treeElements.Add(anchor);
                }
            }

            return treeElements;
        }

        private bool b;
        private void DrawingOnOnDraw(EventArgs args)
        {
            if (b)
                return;
            if (!RivenMenu.Draw["drawDependencies"].Cast<CheckBox>().CurrentValue)
                return;

            //c = true;

            DrawSimpleBranches();
        }

        private void DrawSimpleBranches()
        {
            foreach (var anchor in GetTree())
            {
                Images[anchor.RealAnchor.CastType].Draw(anchor.RealAnchor.Position);

                foreach (var sub in anchor.Subs)
                {

                    Images[sub.CastType].Draw(sub.Position);
                    var from = new Vector2(anchor.RealAnchor.Position.X + 32, anchor.RealAnchor.Position.Y);
                    var to = new Vector2(sub.Position.X + 32, sub.Position.Y);
                    Drawing.DrawLine(from, to, 3, Color.DodgerBlue);
                }
            }
        }

        //private void DrawTree()
        //{
        //    var tree = GetTree();
        //    if (!tree.Any())
        //        return;

        //    int o = -64 / 2;

        //    float baseStep = Drawing.Height * 0.13888888888f;

        //    Images[tree[0].MainCastType].Draw(new Vector2(Drawing.Width / 10f - o, baseStep));
        //    tree[0].RenderX = Drawing.Width / 10f;

        //    for (int level = 2; level <= 10; level++)
        //    {
        //        float currentY = level * baseStep;
        //        float lastY = currentY - baseStep;

        //        var lvl = level;
        //        var childsInSameLevel = GetChildsInSameLevel(tree, lvl);
        //        var sameParentRelations = GetSameParents(childsInSameLevel);
        //        int childAmountInLevel = childsInSameLevel.Count;

        //        foreach (var relation in sameParentRelations)
        //        {
        //            float parentX = relation.Childs[0].Parents.Last().RenderX;
        //            if (parentX == 0)
        //                parentX = tree[0].RenderX - o;

        //            if (relation.IsSingleRelation)
        //            {
        //                var childPos = new Vector2(parentX, currentY);
        //                Images[relation.Childs[0].MainCastType].Draw(childPos);
        //                Drawing.DrawLine(new Vector2(parentX - o, lastY - o),
        //                        new Vector2(childPos.X - o, childPos.Y - o), 3, System.Drawing.Color.DodgerBlue);

        //                /*set child render X when being a parent*/
        //                relation.Childs[0].RenderX = childPos.X;
        //            }
        //            else
        //            {
        //                float baseX = Drawing.Width * 1.5f - 0.48518518518f * Drawing.Width * (level - 2);
        //                float step = baseX / childAmountInLevel;
        //                float currentChildX = parentX - 400 / 2f / childAmountInLevel;

        //                var alreadyOccuredChilds = new List<TreeElement>();
        //                foreach (var relationChild in relation.Childs)
        //                {
        //                    if (alreadyOccuredChilds.Any(x => x.MainCastType == relationChild.MainCastType && x.Level == relationChild.Level))
        //                        continue;

        //                    Vector2 childPos = new Vector2(currentChildX - o, currentY);
        //                    Images[relationChild.MainCastType].Draw(childPos);

        //                    Drawing.DrawLine(new Vector2(parentX - o, lastY - o),
        //                        new Vector2(childPos.X - o, childPos.Y - o), 3, System.Drawing.Color.DodgerBlue);
        //                    currentChildX += step;

        //                    /*set child render X when being a parent*/
        //                    relationChild.RenderX = childPos.X;
        //                    alreadyOccuredChilds.Add(relationChild);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
