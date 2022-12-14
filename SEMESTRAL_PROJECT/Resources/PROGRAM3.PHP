<?php
// Program to compute sum of numbers up to defined one

function sum(int $n)
{
    int $reti = $n;
    if ($n != 0)
    {
        $n--;
        int $res = sum($n);
        $reti = $reti + $res;
    }
    return $reti;
}
echo("Enter number:");
string $input = fgets(STDIN);
int $val = intval($input);
if ($val > 0)
{
    int $result = sum($val);
    echo(strval($result));
}
?>
