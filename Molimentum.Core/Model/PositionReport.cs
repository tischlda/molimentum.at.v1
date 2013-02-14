using System;

namespace Molimentum.Model
{
    public class PositionReport : IItem, IEditablePosition
    {
        public virtual string ID { get; set; }

        public virtual string Comment { get; set; }

        public virtual DateTime? PositionDateTime { get; set; }

        public virtual Position Position { get; set; }

        public virtual float Course { get; set; }

        public virtual float Speed { get; set; }

        public virtual float WindDirection { get; set; }

        public virtual float WindSpeed { get; set; }
    }
}