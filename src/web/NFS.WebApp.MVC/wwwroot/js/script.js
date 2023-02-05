// some scripts

// jquery ready start
$(document).ready(function() {
    // jQuery code
    console.log("teste");



    
    /* ///////////////////////////////////////

    THESE FOLLOWING SCRIPTS ONLY FOR BASIC USAGE, 
    For sliders, interactions and other

    */ ///////////////////////////////////////
    

	//////////////////////// Prevent closing from click inside dropdown
    $(document).on('click', '.dropdown-menu', function (e) {
      e.stopPropagation();
    });


    

	//////////////////////// Bootstrap tooltip
    if ($('[data-toggle="dropdown"]').length>0) {  // check if element exists
        //$('[data-toggle="dropdown"]').tooltip()
	} // end if




    
}); 
// jquery end

