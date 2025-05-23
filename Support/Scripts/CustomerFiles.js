﻿var ViewModel = function () {
    var self = this;
    var CustomerFilesUri = server + '/api/Data/CustomerFiles/'; // آدرس فایل
    self.CustomerFilesList = ko.observableArray([]); // ليست فایل
    var CustomerDownloadlUri = server + '/api/Data/CustomerDocumentsDownload/'; // آدرس دانلود
    var CustomerDownload_TesttUri = server + '/api/Data/CustomerDownload_Test/'; // آدرس تست دانلود

    if (lockNumber == null) {
        window.location.href = localStorage.getItem("urlLogin");
    }

    $("#Index_TextLogo").text('دریافت فایل');

    function getCustomerFilesList(Log) {
        var CustomerFilesObject = {
            LockNumber: lockNumber,
            FlagLog: Log,
            IP: ipw,
            CallProg: 'Web'
        }
        ajaxFunction(CustomerFilesUri, 'POST', CustomerFilesObject, true).done(function (data) {
            self.CustomerFilesList(data == null ? [] : data);
        });
    }
    getCustomerFilesList(true);


    self.Download = function (item) {
        ajaxFunction(CustomerDownload_TesttUri + item.Id, 'GET', true).done(function (data) {
            if (data == "NotFound") {
                return showNotification('فایل یافت نشد', 0);
            }
            else {
                Swal.fire({
                    title: 'تایید دانلود',
                    text: "آیا فایل انتخابی دانلود شود ؟",
                    type: 'warning',
                    showCancelButton: true,
                    cancelButtonColor: '#3085d6',
                    cancelButtonText: 'خیر',
                    allowOutsideClick: false,
                    confirmButtonColor: '#d33',
                    confirmButtonText: 'بله'
                }).then((result) => {
                    if (result.value) {
                        var a = document.createElement("a");
                        a.href = CustomerDownloadlUri + item.LockNumber + '/' + item.Id + '/' + ipw.replaceAll('.', '-') + '/Web';
                        a.click();
                        setTimeout(function () {
                            getCustomerFilesList(false);
                        }, 2000);
                    }
                });
            }
        });
    }

    self.ReplaceSpec = function (item) {
        return item.replace(/<\/?[^>]+(>|$)/g, "");
    }

    self.ReplaceDate = function (item) {
        date = item.substring(0, 10);
        date = date.replaceAll('-', '/');
        return convertDate(date);
    }

    self.ReplaceTime = function (item) {
        return item.substring(11, 100);
    }



    $('#refreshCustomerFile').click(function () {
        Swal.fire({
            title: 'تایید به روز رسانی',
            text: "لیست فایل ها به روز رسانی شود ؟",
            type: 'info',
            showCancelButton: true,
            cancelButtonColor: '#3085d6',
            cancelButtonText: 'خیر',

            confirmButtonColor: '#d33',
            confirmButtonText: 'بله'
        }).then((result) => {
            if (result.value) {
                getCustomerFilesList(false);
            }
        })
    })


};


ko.applyBindings(new ViewModel());




