using Business.Abstract;
using Business.DependencyResolvers.Ninject;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebFormsUI
{
    public partial class Form1 : Form
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        public Form1()
        {
            InitializeComponent();
            _categoryService = InstanceFactory.GetInstance<ICategoryService>();
            _productService = InstanceFactory.GetInstance<IProductService>();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadProducts();
        }

        private void LoadCategories()
        {
            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryId";

            cbxCategoryId.DataSource = _categoryService.GetAll();
            cbxCategoryId.DisplayMember = "CategoryName";
            cbxCategoryId.ValueMember = "CategoryId";

            cbxCategoryIdUpdate.DataSource = _categoryService.GetAll();
            cbxCategoryIdUpdate.DisplayMember = "CategoryName";
            cbxCategoryIdUpdate.ValueMember = "CategoryId";

            lbxCategory.DataSource = _categoryService.GetAll();
            lbxCategory.DisplayMember = "CategoryName";
            lbxCategory.ValueMember = "CategoryId";

            lblCategoryCount.Text = lbxCategory.Items.Count.ToString();

        }

        private void LoadProducts()
        {
            dgwProduct.DataSource = _productService.GetAll();
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgwProduct.DataSource = _productService.GetProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue));
            }
            catch 
            {
            }
        }

        private void tbxProductName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxProductName.Text))
            {
                dgwProduct.DataSource = _productService.GetProductsByProductName(tbxProductName.Text);
            }
            else
            {
                LoadProducts();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Add(new Product
                {
                    CategoryId = Convert.ToInt32(cbxCategoryId.SelectedValue),
                    ProductName = tbxNewProductName.Text,
                    QuantityPerUnit = tbxQuantityPerUnit.Text,
                    UnitPrice = Convert.ToDecimal(tbxQuantityPerUnit.Text),
                    UnitsInStock = Convert.ToInt16(tbxStock.Text)
                });
                MessageBox.Show("Ürün Eklendi");
                LoadProducts();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Update(new Product
                {
                    ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value),
                    ProductName = tbxUpdateProductName.Text,
                    CategoryId = Convert.ToInt32(cbxCategoryIdUpdate.SelectedValue),
                    QuantityPerUnit = tbxQuantityPerUnitUpdate.Text,
                    UnitPrice = Convert.ToDecimal(tbxUnitPriceUpdate.Text),
                    UnitsInStock = Convert.ToInt16(tbxStockUpdate.Text)
                });

                MessageBox.Show("Ürün Güncellendi");
                LoadProducts();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var currentRow = dgwProduct.CurrentRow;
            cbxCategoryIdUpdate.SelectedValue = currentRow.Cells[1].Value;
            tbxUpdateProductName.Text = currentRow.Cells[2].Value.ToString();
            tbxUnitPriceUpdate.Text = currentRow.Cells[3].Value.ToString();
            tbxQuantityPerUnitUpdate.Text = currentRow.Cells[4].Value.ToString();
            tbxStockUpdate.Text = currentRow.Cells[5].Value.ToString();
            //tbxUpdateCategoryName.Text = currentRow.Cells[1].Displayed.ToString();

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgwProduct.CurrentRow != null)
            {
                try
                {
                    _productService.Delete(new Product { ProductId = Convert.ToInt32(dgwProduct.CurrentRow.Cells[0].Value) });
                    MessageBox.Show("Ürün Silindi !");
                    LoadProducts();

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void btnCategoryAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _categoryService.Add(new Category
                {
                    CategoryName = tbxNewCategoryName.Text
                });
                MessageBox.Show("Kategori Eklendi");
                LoadCategories();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }


        private void lbxCategory_Click(object sender, EventArgs e)
        {
            tbxUpdateCategoryName.Text = lbxCategory.Text;
        }

        private void btnCategoryUpdate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxUpdateCategoryName.Text))
            {
                try
                {
                    _categoryService.Update(new Category
                    {
                        CategoryId = Convert.ToInt32(lbxCategory.SelectedValue),
                        CategoryName = tbxUpdateCategoryName.Text
                    });
                    MessageBox.Show("Kategori Güncellendi");
                    tbxUpdateCategoryName.Text = "";
                    LoadCategories();

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen Kategori Seçin !");
            }
        }

        private void btnCategoryRemove_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxUpdateCategoryName.Text))
            {
                try
                {
                    _categoryService.Delete(new Category { CategoryId = Convert.ToInt32(lbxCategory.SelectedValue) });
                    MessageBox.Show("Kategori Silindi");
                    tbxUpdateCategoryName.Text = "";
                    LoadCategories();

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Lütfen Kategori Seçin !");
            }
        }
    }
}
