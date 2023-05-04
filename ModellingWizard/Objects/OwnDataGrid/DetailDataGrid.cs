using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModellingWizard.Objects.OwnDataGrid
{
    public class DetailDataGrid
    {
        public static List<DataGridTextColumn> Get()
        {
            List<DataGridTextColumn> AllColums = new()
            {
                /* Name Column */
                new()
                {
                    Header = "Attribute Name",
                    IsReadOnly = true,
                    Binding = new Binding { Path = new PropertyPath("Name"), Mode = BindingMode.TwoWay }
                },
                /* Value Column */
                new()
                {
                    Header = "Value",
                    IsReadOnly = false,
                    Binding = new Binding { Path = new PropertyPath("Value"), Mode = BindingMode.TwoWay }
                }
            };

            /* Colums for expert mode */
            if (Instances.ExpertMode)
            {
                /* Default Column */
                AllColums.Add(
                    new()
                    {
                        Header = "Default",
                        IsReadOnly = true,
                        Binding = new Binding { Path = new PropertyPath("Default"), Mode = BindingMode.TwoWay }
                    }
                );
                /* Units Column */
                AllColums.Add(
                    new()
                    {
                        Header = "Units",
                        IsReadOnly = true,
                        Binding = new Binding { Path = new PropertyPath("Unit"), Mode = BindingMode.TwoWay }
                    }
                );
                /* DataType Column */
                AllColums.Add(
                    new()
                    {
                        Header = "DataType",
                        IsReadOnly = true,
                        Binding = new Binding { Path = new PropertyPath("DataType"), Mode = BindingMode.TwoWay }
                    }
                );
                /* Semantic Column */
                AllColums.Add(
                    new()
                    {
                        Header = "Semantic",
                        IsReadOnly = true,
                        Binding = new Binding { Path = new PropertyPath("Semantic"), Mode = BindingMode.TwoWay }
                    }
                );
            }

            /* Description Column */
            AllColums.Add( new()
            {
                Header = "Description",
                IsReadOnly = true,
                Binding = new Binding { Path = new PropertyPath("Description"), Mode = BindingMode.TwoWay }
            });
            return AllColums;
        }
    }
}
