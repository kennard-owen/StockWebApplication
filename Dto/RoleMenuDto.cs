using System;
using System.Collections.Generic;
using System.Text;
using PF.Core.Entity;

namespace PF.Core.Dto
{
    public class RoleMenuDto
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleType { get; set; }
        public string Remark { get; set; }

        public List<Sys_rolemenu> Children { get; set; }
    }
}