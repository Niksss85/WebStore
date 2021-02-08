using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.ViewModels;

namespace WebStore.Models
{
    public interface ICartService
    {
        void Add(int id);
        void Decrement(int id);

        void Remove(int id);
        void Clear();
        CartViewModel GetViewModel();
    }
}
