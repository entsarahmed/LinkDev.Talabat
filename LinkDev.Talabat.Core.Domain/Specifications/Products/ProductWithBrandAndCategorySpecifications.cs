﻿using LinkDev.Talabat.Core.Domain.Entities.Products;
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
        //The Object Created via this Constructor is used for building the query that will Get all product

        public ProductWithBrandAndCategorySpecifications():base()
        {
            AddInclude();
        }

        private void AddInclude()
        {
            Includes.Add(P => P.Brand!);
            Includes.Add(P => P.Category!);
        }

        //The Spec Object Created via this Constructor is used for building the query that will Get a Specific Product

        public ProductWithBrandAndCategorySpecifications(int id):base(id) 
        {
            AddInclude();

        }
    }
}