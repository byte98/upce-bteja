<?php
// Program to compute sum of numbers up to defined one

function sum(int $n)
{
    if ($n != 0)
    {
        $n--;
        int $reti = 0;
        $reti = sum($n);
        $reti = $n + $reti;
        return $reti;
    }
    else
    {
        return $n;
    }
}
echo("Enter number:");
string $input = fgets(STDIN);
int $val = intval($input);
if ($val > 0)
{
    int $result = 0;
    $result = sum($val);
    echo($result);
}
?>