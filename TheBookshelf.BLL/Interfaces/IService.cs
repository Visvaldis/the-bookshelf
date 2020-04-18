using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.Interfaces
{
	public interface IService<T> where T : class
	{
		IEnumerable<T> GetAll();
		T Get(int id);
		IEnumerable<T> GetWithFilter(Func<T, Boolean> filter);
		void Add(T item);
		void Update(T item);
		void Delete(int id);
		void Dispose();
	}
}
