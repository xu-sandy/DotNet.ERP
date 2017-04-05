var pharos = pharos || {};
(function (para) {
    para.gridHelper = {
        reloadGrid: function (grid, url) {
            var queryParams = $("body").GetPostData();
            if (queryParams != null) {
                grid.datagrid('options').queryParams = queryParams;
            }
            if (url != null) {
                grid.datagrid('options').url = url;
            }
            grid.datagrid('reload');
        },
        getSelectedRow: function (grid) {
            var row = grid.datagrid("getSelected");
            if (row) {
                return row;
            }
            else {
                $.messager.alert('Message', 'Please select the row you want to operate!'); //请先选中要操作的行
                return false;
            }
        },
        formatColumn: function formatColumn(value, list) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].Value == value) {
                    if (list[i].Value != "")
                        return list[i].Text;
                }
            }
            return value;
        }
    };
})(pharos);