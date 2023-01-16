using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBaseDetail<DetailViewModel>
        where DetailViewModel:class,new()
    {
        Task<DetailViewModel> GetById(long id);
    }
}
