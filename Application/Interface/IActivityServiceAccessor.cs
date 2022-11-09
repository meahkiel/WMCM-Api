using Application.DTO;
using Core.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IActivityServiceAccessor
    {

        Task<Activity> Execute(ActivityEntryDTO dto);
    }
}
