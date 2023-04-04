using System;

namespace FillColorGame.GridComponents
{
    [Flags]
    public enum MarkerType : byte
    {
        None = 0,
        PathFinderMarker = 1,
        ColorChangingMarker = 2
    }
}

