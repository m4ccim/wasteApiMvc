using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI
{
    public static class Detacher
    {
        public static void DetachLocal<T>(this ModelsContext context, T t, int entryId)
    where T : class, IIdentifier
        {
            var local = context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.ProductId.Equals(entryId));
            if (local != null)
            {
                context.Entry(local).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            context.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }

    public interface IIdentifier
    {
        int ProductId { get; set; }

    }
}
