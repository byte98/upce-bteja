<?php
// Program which prints numbers from one to defined one
// and at the end computes some mathematical expression
string $input = fgets(STDIN);
int $counter = intval($input);
int $i = 1;
while(1 == 1)
{
    echo($i);
    $i++;
    if ($i > $counter)
    {
        break;
    }
}
int $res = 0;
$res = 1 + 2 * 3 / 4;
?>
