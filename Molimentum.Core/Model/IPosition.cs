using System;

namespace Molimentum.Model
{
    public interface IPosition
    {
        DateTime? PositionDateTime { get; }

        Position Position { get; }
    }
}