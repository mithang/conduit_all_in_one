using System;
using System.Collections.Generic;
using System.Text;
using Conduit.Common.Entity;

namespace Conduit.Domain
{
    public class Note : BaseEntity
    {
        public Note()
        {
        }

        public string Content { get; set; }
    }
}
