﻿using CoffeeShop.Model;
using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeShop.ViewModels
{
    public class WarehouseViewModel : BaseViewModel
    {
        #region variables
        private static WarehouseViewModel _instance = null;
        private static object m_lock = new object();

        private bool _isOpenAddRawMaterialDialog;
        private bool _isOpenChangeAmountRawMaterialDialog;
        private bool _isOpenDeleteRawMaterialDialog;
        private string _newRawMaterialName;
        private int _newRawMaterialAmount;
        private string _newRawMaterialUnit;
        private AsyncObservableCollection<dynamic> _rawMaterials;
        private dynamic _selectedRawMaterial;
        private int _presentRawMaterialAmount;
        private int _changeRawMaterialAmount;

        #endregion

        #region properties
        public bool IsOpenAddRawMaterialDialog { get => _isOpenAddRawMaterialDialog; set { _isOpenAddRawMaterialDialog = value;
                NewRawMaterialName = "";
                NewRawMaterialAmount = 0;
                NewRawMaterialUnit = "";
                OnPropertyChanged(); } }
        public bool IsOpenChangeAmountRawMaterialDialog
        {
            get => _isOpenChangeAmountRawMaterialDialog; set
            {
                _isOpenChangeAmountRawMaterialDialog = value;
                if (value == true)
                {
                    PresentRawMaterialAmount = SelectedRawMaterial.SoLuong;
                    ChangeRawMaterialAmount = 0;
                }
                OnPropertyChanged();
            }
        }
        public bool IsOpenDeleteRawMaterialDialog { get => _isOpenDeleteRawMaterialDialog; set { _isOpenDeleteRawMaterialDialog = value; OnPropertyChanged(); } }
        public string NewRawMaterialName { get => _newRawMaterialName; set { value = value.ToUpper(); _newRawMaterialName = value; OnPropertyChanged(); } }
        public int NewRawMaterialAmount { get => _newRawMaterialAmount; set { _newRawMaterialAmount = value; OnPropertyChanged(); } }
        public string NewRawMaterialUnit { get => _newRawMaterialUnit; set { value = value.ToUpper(); _newRawMaterialUnit = value; OnPropertyChanged(); } }
        public AsyncObservableCollection<dynamic> RawMaterials { get => _rawMaterials; set { _rawMaterials = value; OnPropertyChanged(); } }
        public dynamic SelectedRawMaterial { get => _selectedRawMaterial; set { _selectedRawMaterial = value; OnPropertyChanged(); } }
        public int PresentRawMaterialAmount { get => _presentRawMaterialAmount; set { _presentRawMaterialAmount = value; OnPropertyChanged(); } }
        public int ChangeRawMaterialAmount { get => _changeRawMaterialAmount; set { _changeRawMaterialAmount = value; OnPropertyChanged(); } }

        #endregion

        #region Commands
        public ICommand AddRawMaterialCommand { get; set; }
        public ICommand ClickAddRawMaterialButtonCommand { get; set; }
        public ICommand ChangeAmountRawMaterialCommand { get; set; }
        public ICommand ClickChangeAmountRawMaterialCommand { get; set; }
        public ICommand DeleteRawMaterialCommand { get; set; }
        public ICommand ClickDeleteRawMaterialCommand { get; set; }

        #endregion

        public static WarehouseViewModel GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new WarehouseViewModel();
                    }
                }
            }
            return _instance;
        }

        public WarehouseViewModel()
        {
            // Setup dữ liệu
            IsOpenAddRawMaterialDialog = false;
            // Danh sách nguyên liệu trong kho
            RawMaterials = new AsyncObservableCollection<dynamic>();
            foreach (var item in DataProvider.Ins.DB.KhoNguyenLieu)
            {
                RawMaterials.Add(new { 
                    Ten = item.Ten,
                    SoLuong = item.SoLuong,
                    DonVi = item.DonVi
                });
            }

            //Command
            ClickAddRawMaterialButtonCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                IsOpenAddRawMaterialDialog = true;
            });

            AddRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                if (bool.Parse(param.ToString()) == true)
                {
                    KhoNguyenLieu newRawMaterial = new KhoNguyenLieu {
                        MaNL = (DataProvider.Ins.DB.KhoNguyenLieu.Count() == 0) ? 1 : DataProvider.Ins.DB.KhoNguyenLieu.Max(x => x.MaNL) + 1,
                        Ten = NewRawMaterialName,
                        SoLuong = NewRawMaterialAmount,
                        DonVi = NewRawMaterialUnit
                    };

                    DataProvider.Ins.DB.KhoNguyenLieu.Add(newRawMaterial);
                    DataProvider.Ins.DB.SaveChanges();

                    RawMaterials.Add(new KhoNguyenLieu
                    {
                        Ten = NewRawMaterialName,
                        SoLuong = NewRawMaterialAmount,
                        DonVi = NewRawMaterialUnit
                    });
                }
                IsOpenAddRawMaterialDialog = false;
            });

            ClickChangeAmountRawMaterialCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) =>
            {
                SelectedRawMaterial = param;
                IsOpenChangeAmountRawMaterialDialog = true;
            });

            ChangeAmountRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                if (bool.Parse(param.ToString()) == true)
                {
                    string ten = SelectedRawMaterial.Ten;
                    int soluong = SelectedRawMaterial.SoLuong;
                    DataProvider.Ins.DB.KhoNguyenLieu.First(x => x.Ten == ten).SoLuong += ChangeRawMaterialAmount;
                    DataProvider.Ins.DB.SaveChanges();
                    RawMaterials.Add(new { 
                        Ten = SelectedRawMaterial.Ten,
                        SoLuong = soluong + ChangeRawMaterialAmount,
                        DonVi = SelectedRawMaterial.DonVi
                    });
                    RawMaterials.Remove(SelectedRawMaterial);
                }
                IsOpenChangeAmountRawMaterialDialog = false;
            });

            ClickDeleteRawMaterialCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) =>
            {
                SelectedRawMaterial = param;
                IsOpenDeleteRawMaterialDialog = true;
            });

            DeleteRawMaterialCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                if (bool.Parse(param.ToString()) == true)
                {
                    string ten = SelectedRawMaterial.Ten;
                    KhoNguyenLieu deletekhonguyenlieu = DataProvider.Ins.DB.KhoNguyenLieu.First(x => x.Ten == ten);
                    DataProvider.Ins.DB.KhoNguyenLieu.Remove(deletekhonguyenlieu);
                    DataProvider.Ins.DB.SaveChanges();
                    RawMaterials.Remove(SelectedRawMaterial);
                }
                IsOpenDeleteRawMaterialDialog = false;
            });
        }
    }
}
