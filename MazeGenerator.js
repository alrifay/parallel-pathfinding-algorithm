const rows = 7, cols = 6;
for(let i = 0; i < rows; i++){
    let row = "";
    for(let j = 0; j < cols; j++){
        row += Math.random() > 0.79 ? "-1 " : "0 ";
    }
    console.log(row.trim());
}