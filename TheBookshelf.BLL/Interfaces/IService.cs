using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBookshelf.BLL.Interfaces
{
	public interface IService<T> where T : class
	{
		ICollection<T> GetAll();
		T Get(int id);
		ICollection<T> GetWithFilter(Func<T, Boolean> filter);
		int Add(T item);
		void Update(T item);
		void Delete(int id);
		bool Exist(int id);
		bool Exist(string Name);
		void Dispose();
	}
}
