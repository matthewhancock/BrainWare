using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BrainWare.Data {
    public static class Orders {
        public static async Task<IEnumerable<Models.Order>> GetForCompany(int CompanyID) {
            var orders = new Dictionary<int, Models.Order>();
            using (var connection = await Infrastructure.Database.GetConnection()) {
                using (var cmd = new SqlCommand("SELECT order_id,[description] FROM [Order] WHERE Company_ID=@CompanyID ORDER BY 1", connection)) {
                    cmd.Parameters.AddWithValue("@CompanyID", CompanyID);

                    await Infrastructure.Database.ExecuteReader(cmd, r => {
                        var o = new Models.Order() {
                            ID = r.GetInt32(0),
                            Description = r.GetString(1),
                            Items = new List<Models.Order.LineItem>()
                        };
                        orders.Add(o.ID, o);
                    });
                }

                if (orders.Count > 0) { // don't need to query products if there aren't any orders
                    using (var cmd = new SqlCommand("SELECT order_id,op.product_id,p.name,op.price,quantity FROM orderproduct op JOIN Product p ON op.product_id=p.product_id WHERE order_id IN (SELECT order_id FROM [Order] WHERE company_id=@CompanyID)", connection)) {
                        cmd.Parameters.AddWithValue("@CompanyID", CompanyID);

                        await Infrastructure.Database.ExecuteReader(cmd, r => {
                            var orderID = r.GetInt32(0);
                            var li = new Models.Order.LineItem() {
                                Product = new Models.Product() {
                                    ID = r.GetInt32(1),
                                    Name = r.GetString(2)
                                },
                                Price = r.GetDecimal(3),
                                Quantity = r.GetInt32(4)
                            };

                            orders[orderID].Items.Add(li);
                        });
                    }
                }
            }

            return orders.Values;
        }
    }
}
