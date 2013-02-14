using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks.Constraints;
using System.IO;

namespace Molimentum.Tasks.MailClient.Tests
{
    class StreamConstraint : AbstractConstraint
    {
        private byte[] _data;
        
        public StreamConstraint(byte[] data)
        {
            _data = data;
        }

        public override bool Eval(object obj)
        {
            var stream = obj as Stream;

            if (stream == null) return false;
            

            for (int i = 0 ; i < _data.Length ; i++)
            {
                byte b = (byte)stream.ReadByte();

                if (b != _data[i]) return false;
            }

            if (stream.ReadByte() != -1) return false;

            return true;
        }

        public override string Message
        {
            get { throw new NotImplementedException(); }
        }
    }
}
