$.extend($.fn.treegrid.methods, {
    editCell: function (jq, param) {
        return jq.each(function () {
            var opts = $(this).treegrid('options');
            var fields = $(this).treegrid('getColumnFields', true).concat($(this).treegrid('getColumnFields'));
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).treegrid('getColumnOption', fields[i]);
                col.editor1 = col.editor;
                if (fields[i] != param.field) {
                    col.editor = null;
                }
            }
            $(this).treegrid('beginEdit', param.id);
            var ed = $(this).treegrid('getEditor', param);
            if (ed) {
                if ($(ed.target).hasClass('textbox-f')) {
                    $(ed.target).textbox('textbox').select().focus();
                } else {
                    $(ed.target).select().focus();
                }
            }
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).treegrid('getColumnOption', fields[i]);
                col.editor = col.editor1;
            }
        });
    },
    autoMergeCellsGroupby: function (jq, obj) {//指定合并组
        return jq.each(function () {
            //debugger;
            var groupby = obj.groupby, fields = obj.columns;
            if (!groupby) return;
            var target = $(this);
            if (!fields) {
                fields = target.treegrid("getColumnFields");
            }
            var rows = target.treegrid("getData");
            if (rows.length <= 0) return;

            var curTxt = "";
            var tempIndex = 1;
            var temp = {};
            var datas = function (r,idx) {
                for (var i = 0; i < r.children.length; i++) {
                    var cur = r.children[i][groupby];
                    var row = temp[cur] || { index: 0 };
                    if (curTxt == "")
                        curTxt = cur;
                    else if (cur == curTxt) {
                        tempIndex++;
                        row.span = tempIndex;
                        temp[cur] = row;
                    }
                    else {
                        curTxt = cur;
                        tempIndex = 1;
                        row.span = tempIndex;
                        row.index = i + idx;
                        temp[cur] = row;
                    }
                    datas(r.children[i],i);
                }
            };
            for (var i = 0; i < rows.length; i++) {//首级运算
                var cur = rows[i][groupby];
                var row = temp[cur] || { index: 0 };
                if (curTxt == "")
                    curTxt = cur;
                else if (cur == curTxt) {
                    tempIndex++;
                    row.span = tempIndex;
                    temp[cur] = row;
                }
                else {
                    curTxt = cur;
                    tempIndex = 1;
                    row.span = tempIndex;
                    row.index = i;
                    temp[cur] = row;
                }
            }
            for (var i = 0; i < rows.length; i++) {
                datas(rows[i], i+1);
            }
            var panel = target.treegrid("getPanel");

            $.each(temp, function (k, v) {
                for (var j = 0; j < fields.length; j++) {
                    //target.treegrid("mergeCells", {
                    //    index: v.index,
                    //    field: fields[j],　　//合并字段
                    //    rowspan: v.span,
                    //    colspan: null
                    //});
                    if (v.span > 1) {
                        //debugger;
                        panel.find('.datagrid-view2 tr[node-id="' + k + '"]>td[field="' + fields[j] + '"]').first().attr("rowspan", v.span).end().not(":first").hide();
                    }
                }
            });
        });
    },
    getCheckedIds: function (jq, field) {
        var chks = jq.treegrid('getCheckedNodes');
        var panel = jq.treegrid("getPanel");
        var id = field || "Id";
        var inds = $.map(panel.find(".tree-checkbox2").parents("tr"), function (r, i) {
            return $(r).children("[field='" + id + "']").children("div").html();
        });
        var chkIds= $.map(chks, function (r, i) {
            return r[id]+"";
        });
        inds.addRange(chkIds);
        return inds;
    }
})
$.extend($.fn.datagrid.methods, {
    editCell: function (jq, param) {
        return jq.each(function () {
            var opts = $(this).datagrid('options'); 
            var fields = $(this).datagrid('getColumnFields', true).concat($(this).datagrid('getColumnFields'));
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).datagrid('getColumnOption', fields[i]);
                col.editor1 = col.editor;
                var fds = param.field.split(',');
                if (fds.indexOf( fields[i])==-1) {
                    col.editor = null;
                }
            }
            $(this).datagrid('beginEdit', param.index);
            var ed = $(this).datagrid('getEditor', param);
            if (ed) {
                if ($(ed.target).hasClass('textbox-f')) {
                    $(ed.target).textbox('textbox').select().focus();
                } else {
                    $(ed.target).select().focus();
                }
            }
            for (var i = 0; i < fields.length; i++) {
                var col = $(this).datagrid('getColumnOption', fields[i]);
                col.editor = col.editor1;
            }
        });
    },
    enableCellEditing: function (jq) {
        return jq.each(function () {
            var grid = $(this);
            var opts = grid.datagrid('options');
            opts.oldOnClickCell = opts.onClickCell;
            opts.onClickCell = function (index, field) {
                if (opts.editIndex != undefined) {
                    if (grid.datagrid('validateRow', opts.editIndex)) {
                        grid.datagrid('endEdit', opts.editIndex);
                        opts.editIndex = undefined;
                    } else {
                        return;
                    }
                }
                grid.datagrid('selectRow', index).datagrid('editCell', {
                    index: index,
                    field: field
                });
                opts.editIndex = index;
                opts.oldOnClickCell.call(this, index, field);
            }
        });
    },
    autoMergeCells: function (jq, fields) {
        return jq.each(function () {
            var target = $(this);
            if (!fields) {
                fields = target.datagrid("getColumnFields");
            }
            var rows = target.datagrid("getRows");
            var i = 0,
			j = 0,
			temp = {};
            for (i; i < rows.length; i++) {
                var row = rows[i];
                j = 0;
                for (j; j < fields.length; j++) {
                    var field = fields[j];
                    var tf = temp[field];
                    if (!tf) {
                        tf = temp[field] = {};
                        tf[row[field]] = [i];
                    } else {
                        var tfv = tf[row[field]];
                        if (tfv) {
                            tfv.push(i);
                        } else {
                            tfv = tf[row[field]] = [i];
                        }
                    }
                }
            }
            $.each(temp, function (field, colunm) {
                $.each(colunm, function () {
                    var group = this;

                    if (group.length > 1) {
                        var before,
						after,
						megerIndex = group[0];
                        for (var i = 0; i < group.length; i++) {
                            before = group[i];
                            after = group[i + 1];
                            if (after && (after - before) == 1) {
                                continue;
                            }
                            var rowspan = before - megerIndex + 1;
                            if (rowspan > 1) {
                                target.datagrid('mergeCells', {
                                    index: megerIndex,
                                    field: field,
                                    rowspan: rowspan
                                });
                            }
                            if (after && (after - before) != 1) {
                                megerIndex = after;
                            }
                        }
                    }
                });
            });
        });
    },
    autoMergeCellsGroupby: function (jq, obj) {//指定合并组
        return jq.each(function () {
            //debugger;
            var groupby = obj.groupby, fields = obj.columns;
            if (!groupby) return;
            var target = $(this);
            if (!fields) {
                fields = target.datagrid("getColumnFields");
            }
            var rows = target.datagrid("getRows");
            if (rows.length <= 0) return;

            var curTxt = "";
            var tempIndex = 1;
            var temp = {};
            for (var i = 0; i < rows.length; i++) {
                var cur = rows[i][groupby];
                var row = temp[cur]||{index:0};
                if (curTxt == "")  
                    curTxt = cur;
                else if (cur == curTxt) {
                    tempIndex++;
                    row.span = tempIndex;
                    temp[cur] = row;
                }
                else {
                    curTxt = cur;
                    tempIndex = 1;
                    row.span = tempIndex;
                    row.index = i;
                    temp[cur] = row;
                }
            }
            $.each(temp, function (k,v) {
                for (var j = 0; j < fields.length; j++) {
                    target.datagrid("mergeCells", {
                        index: v.index,
                        field: fields[j],　　//合并字段
                        rowspan: v.span,
                        colspan: null
                    });
                }
            });
        });
    },
    setColumnTitle: function (jq, option) {//动态设置列标题
        if (option.field) {
            return jq.each(function () {
                var $panel = $(this).datagrid("getPanel");
                var $field = $('td[field=' + option.field + ']', $panel);
                if ($field.length) {
                    var $span = $("span", $field).eq(0);
                    $span.html(option.text);
                }
            });
        }
        return jq;
    }

});
var errNum = 0;
function loadError() {
    if (errNum == 3) {
        alert("加载失败,请关闭页面重试!");
        errNum = 0;
    } else {
        $(this).datagrid("reload");
        errNum++;
    }
}
//修改easyui-combobox默认下拉面板属性
$.extend($.fn.combobox.defaults, {
    panelHeight: 'auto',
    panelMaxHeight: 200,
})