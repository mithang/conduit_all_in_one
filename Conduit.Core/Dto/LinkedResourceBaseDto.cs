using System.Collections.Generic;

namespace Conduit.Common.Dto
{
    public abstract class LinkedResourceBaseDto
    {
        public List<LinkDto> Links { get; set; }
        = new List<LinkDto>();
    }
}
