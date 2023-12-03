using System.Text.RegularExpressions;

string[] schem = File.ReadLines("input").ToArray();

int width = schem[0].Length;
int height = schem.Length;
int sum=0;

//maps potential gear locations to 1st number seen. When 2nd number seen, gets added to gear sum
var gears = new Dictionary<(int, int), int>();
int gearsum = 0;

for (int line=0; line < height; line++) {
    foreach (Match num_match in Regex.Matches(schem[line],@"\d+")) {
        int val = int.Parse(num_match.Value);
        if (CheckForSymbol(line, num_match.Index, num_match.Index+num_match.Length-1,val))
            sum+= val;
    }
}

Console.WriteLine(sum);
Console.WriteLine(gearsum);

bool CheckForSymbol(int line, int charstart, int charend, int value) {
    int start_row = line == 0 ? 0 : line-1;
    int end_row = line == height-1 ? height-1 : line+1;
    int start_col = charstart == 0 ? 0 : charstart-1;
    int end_col = charend == width-1 ? width-1 : charend+1;
    bool foundsymb = false;

    for (int r = start_row; r <= end_row; r++) {
        for (int c = start_col; c <= end_col; c++) {
            if( !Char.IsDigit(schem[r][c]) && schem[r][c] != '.')
                foundsymb = true;
            if (schem[r][c] == '*') {
                if (gears.ContainsKey((r,c))) {
                    gearsum += gears[(r,c)]*value;
                } else {
                    gears[(r,c)] = value;
                }
            }
        }
    }

    return foundsymb;
}
