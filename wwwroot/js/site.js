// Write your Javascript code.
var olp = 0;
$( "#sortable" ).sortable({
    start: function(event, ui) { 
        olp = ui.item.index();
        console.log('start: ' + ui.item.index())
    },
    update: function(event, ui) {
        let oldposition = olp;
        let newOrder = ui.item.index();    
        var projId = document.getElementById("help").value;   
        console.log("ProjectId: " + projId);
        console.log("New position: " + newOrder);

        $.ajax({
            url: `/project/updateSceneOrder/${projId}`,
            type: 'POST',
            data: {
               newIndex: newOrder, 
               oldIndex: oldposition
            },
            dataType: 'application/json',
            success: function (data, anything, anything2) {
                console.log(data);
                console.log(data.success);
            }   
        })
        document.location.reload();
}});

$( "#sortable" ).disableSelection();
