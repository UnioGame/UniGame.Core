﻿namespace UniModules.UniUiSystem.Runtime.Utils
{
    using UnityEngine;

    public static class CanvasGroupExtension
    {
        public static CanvasGroupState Disabled = new CanvasGroupState();
        public static CanvasGroupState Enabled  = new CanvasGroupState() {
            Alpha         = 1,
            Interactable  = true,
            IgnoreParent  = false,
            BlockRaycasts = true
        };
        
        public static CanvasGroupState SetState(
            this CanvasGroup group, 
            float alpha,
            bool             interactable  = true,
            bool             blockRaycasts = true,
            bool             ignoreParent  = false)
        {
            if(group == null) return Disabled;
            
            var state = GetState(group);

            var resultState = CreateState(alpha, interactable, blockRaycasts, ignoreParent);
            group.SetState(resultState);

            return state;
        }

        public static CanvasGroupState SetBlocksRaycast(this CanvasGroup group, bool blockRaycast)
        {
            if(group == null) return Disabled;
            var state = GetState(group);
            state.BlockRaycasts = blockRaycast;
            SetState(group,state);
            return state;
        }

        public static CanvasGroupState CreateState(
            float alpha,
            bool  interactable  = true,
            bool  blockRaycasts = true,
            bool  ignoreParent  = false)
        {
            
            var resultState = new CanvasGroupState() {
                Alpha         = alpha,
                Interactable  = interactable,
                IgnoreParent  = ignoreParent,
                BlockRaycasts = blockRaycasts,
            };
            return resultState;
        }

        public static CanvasGroupState GetState(this CanvasGroup group)
        {
            if(!group) return Disabled;
            
            return new CanvasGroupState() {
                Alpha         = group.alpha,
                Interactable  = group.interactable,
                IgnoreParent  = group.ignoreParentGroups,
                BlockRaycasts = group.blocksRaycasts
            };
        }

        public static void SetState(this CanvasGroup group, CanvasGroupState state)
        {
            if(!group)
                return;
            
            group.alpha              = state.Alpha;
            group.interactable       = state.Interactable;
            group.ignoreParentGroups = state.IgnoreParent;
            group.blocksRaycasts     = state.BlockRaycasts;
        }
    }
}