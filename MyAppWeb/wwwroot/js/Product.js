var dtable;

$(document).ready(function () {
    dtable=$('#myTable').DataTable({

        "ajax": {
            "url": "/Admin/Product/AllProducts"
        },

        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data":  "price"},
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                         <a href="/Admin/Product/CreateUpdate?id=${data}"  class="btn btn-info"><i class="bi bi-pencil-square"></i>  Edit</a> &nbsp;
                         <a  href="/Admin/Product/Delete?id=${data}" type="submit" class="btn btn-danger "><i class="bi bi-trash"></i>  Delete</a>
                    `
                }
            },

        ]
    });
});