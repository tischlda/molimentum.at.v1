using System;

namespace Molimentum.Model
{
    public interface IEditablePosition : IPosition
    {
        new DateTime? PositionDateTime { get; set; }

        new Position Position { get; set; }
    }
}