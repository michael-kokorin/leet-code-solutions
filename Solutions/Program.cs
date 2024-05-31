var board = new string[][]
{
    ["5", "3", ".", ".", "7", ".", ".", ".", "."],
    ["6", ".", ".", "1", "9", "5", ".", ".", "."],
    [".", "9", "8", ".", ".", ".", ".", "6", "."],
    ["8", ".", ".", ".", "6", ".", ".", ".", "3"],
    ["4", ".", ".", "8", ".", "3", ".", ".", "1"],
    ["7", ".", ".", ".", "2", ".", ".", ".", "6"],
    [".", "6", ".", ".", ".", ".", "2", "8", "."],
    [".", ".", ".", "4", "1", "9", ".", ".", "5"],
    [".", ".", ".", ".", "8", ".", ".", "7", "9"]
};

var initArray = new[]
{
    1, 2, 3, 4, 5, 6, 7, 8, 9
};
var rowAllowedValues = new HashSet<int>[9];
var rowAllowedIndices = new HashSet<int>[9];
var columnAllowedValues = new HashSet<int>[9];
var columnAllowedIndices = new HashSet<int>[9];
var squareAllowedValues = new HashSet<int>[9];
var squareAllowedIndices = new HashSet<int>[9];
for (var index = 0; index < 9; index++)
{
    rowAllowedValues[index] = [..initArray];
    rowAllowedIndices[index] = [..initArray];
    columnAllowedValues[index] = [..initArray];
    columnAllowedIndices[index] = [..initArray];
    squareAllowedValues[index] = [..initArray];
    squareAllowedIndices[index] = [..initArray];
}

var remainingCount = 81;
var matrix = new int?[9, 9];
for (var i = 0; i < 9; i++)
{
    for (var j = 0; j < 9; j++)
    {
        if (board[i][j] != ".")
        {
            var value = int.Parse(board[i][j]);
            rowAllowedValues[i].Remove(value);
            rowAllowedIndices[i].Remove(j + 1);
            
            columnAllowedValues[j].Remove(value);
            columnAllowedIndices[j].Remove(i + 1);
            
            var squareIndex = GetSquareIndex(i, j);
            squareAllowedValues[squareIndex].Remove(value);
            squareAllowedIndices[squareIndex].Remove(GetInsideSquareIndex(i, j));

            matrix[i, j] = value;
            remainingCount--;
        }
    }
}

PrintMatrix();

DoStep();

static int GetSquareIndex(int i, int j)
{
    return i / 3 * 3 + j / 3;
}

static int GetInsideSquareIndex(int i, int j)
{
    return i % 3 * 3 + j % 3 + 1;
}

bool DoStep()
{
    if (remainingCount == 0)
    {
        return true;
    }

    var step = GetSingleStep();
    if (step.HasValue)
    {
        var (i, j, val) = step.Value;
        if (!rowAllowedValues[i].Contains(val) || !columnAllowedValues[j].Contains(val) || !squareAllowedValues[GetSquareIndex(i, j)].Contains(val))
        {
            return false;
        }

        Console.WriteLine("Apply single step");

        MakeMove(i, j, val);
        if (DoStep())
        {
            return true;
        }
        
        UndoMove(i, j, val);
        return false;
    }

    var rowIndex = rowAllowedValues.Select((row, index) => new { index, row.Count }).Where(row => row.Count != 0).MinBy(item => item.Count).index;
    var rowIndices = rowAllowedIndices[rowIndex];
    var columnIndex = rowIndices.First() - 1;
    var columnValues = columnAllowedValues[columnIndex];
    var rowValues = rowAllowedValues[rowIndex].ToArray();
    var squareIndex = GetSquareIndex(rowIndex, columnIndex);
    var squareValues = squareAllowedValues[squareIndex];
    foreach (var rowValue in rowValues)
    {
        if (!columnValues.Contains(rowValue))
        {
            continue;
        }

        if (!squareValues.Contains(rowValue))
        {
            continue;
        }

        MakeMove(rowIndex, columnIndex, rowValue);
        if (DoStep())
        {
            return true;
        }
        
        UndoMove(rowIndex, columnIndex, rowValue);
    }

    return false;
}

static int? GetSingleValue(HashSet<int>[] allowedValues)
{
    for (var index = 0; index < allowedValues.Length; index++)
    {
        if (allowedValues[index].Count == 1)
        {
            return index;
        }
    }

    return null;
}

(int i, int j, int val)? GetSingleStep()
{
    var simpleRowIndex = GetSingleValue(rowAllowedValues);
    if (simpleRowIndex.HasValue)
    {
        var j = rowAllowedIndices[simpleRowIndex.Value].First() - 1;
        var value = rowAllowedValues[simpleRowIndex.Value].First();
        return (simpleRowIndex.Value, j, value);
    }
    
    var simpleColumnIndex = GetSingleValue(columnAllowedValues);
    if (simpleColumnIndex.HasValue)
    {
        var i = columnAllowedIndices[simpleColumnIndex.Value].First() - 1;
        var value = columnAllowedValues[simpleColumnIndex.Value].First();
        return (i, simpleColumnIndex.Value, value);
    }
    
    var simpleSquareIndex = GetSingleValue(squareAllowedValues);
    if (simpleSquareIndex.HasValue)
    {
        var indexInsideSquare = squareAllowedIndices[simpleSquareIndex.Value].First() - 1;
        var i = simpleSquareIndex.Value / 3 * 3 + indexInsideSquare / 3;
        var j = (simpleSquareIndex.Value % 3) * 3 + indexInsideSquare % 3;
        var value = squareAllowedValues[simpleSquareIndex.Value].First();
        return (i, j, value);
    }

    return null;
}

void MakeMove(int i, int j, int value)
{
    Console.WriteLine($"Set {i} {j} {value}");
    
    rowAllowedValues[i].Remove(value);
    rowAllowedIndices[i].Remove(j + 1);
    
    columnAllowedValues[j].Remove(value);
    columnAllowedIndices[j].Remove(i + 1);
    
    var squareIndex = GetSquareIndex(i, j);
    squareAllowedValues[squareIndex].Remove(value);
    squareAllowedIndices[squareIndex].Remove(GetInsideSquareIndex(i, j));

    matrix[i, j] = value;
    remainingCount--;

    PrintMatrix();
}

void UndoMove(int i, int j, int value)
{
    Console.WriteLine($"Undo {i} {j} {value}");
    
    rowAllowedValues[i].Add(value);
    rowAllowedIndices[i].Add(j + 1);
    
    columnAllowedValues[j].Add(value);
    columnAllowedIndices[j].Add(i + 1);
    
    var squareIndex = GetSquareIndex(i, j);
    squareAllowedValues[squareIndex].Add(value);
    squareAllowedIndices[squareIndex].Add(GetInsideSquareIndex(i, j));

    matrix[i, j] = null;
    remainingCount++;
    
    PrintMatrix();
}

void PrintMatrix()
{
    for (var i = 0; i < 9; i++)
    {
        for (var j = 0; j < 9; j++)
        {
            Console.Write(matrix[i, j]?.ToString() ?? ".");
            if (j % 3 == 2)
            {
                Console.Write("|");
            }
        }
        
        if (i % 3 == 2)
        {
            Console.WriteLine();
            Console.WriteLine("-----------");
        }
        else
        {
            Console.WriteLine();
        }
    }
}