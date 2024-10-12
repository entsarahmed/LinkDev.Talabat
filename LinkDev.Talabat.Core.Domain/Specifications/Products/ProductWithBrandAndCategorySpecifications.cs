using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Specifications.Products
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product,int>
    {
        //The Spec Object Created via this Constructor is used for building the query that will Get all product

        public ProductWithBrandAndCategorySpecifications(string? sort):base()
        {
            AddInclude();
            AddOrderBy(P => P.Name); //Default
            if(!string.IsNullOrEmpty(sort))
            {
                switch(sort)
                {
                    case "nameDesc":
                        AddOrderByDesc(P => P.Name);
                        break;

                    case "priceAsc":
                        AddOrderBy(P => P.Price);   
                        break;

                    case "priceDesc":
                        AddOrderByDesc(P => P.Price); 
                        break;

                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            
        }


        //The Spec Object Created via this Constructor is used for building the query that will Get a Specific Product

        public ProductWithBrandAndCategorySpecifications(int id):base(id) 
        {
            AddInclude();

        }
        private protected override void AddInclude()
        {
            base.AddInclude();
            
            Includes.Add(P => P.Brand!);
            Includes.Add(P => P.Category!);
        }

    }
}
