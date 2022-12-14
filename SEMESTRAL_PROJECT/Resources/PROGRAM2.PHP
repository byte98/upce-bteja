<?php
// Program which computes power of real number

// Function which computes power of real number
function exp(float $base, int $exponent)
{
    float $reti = $base;
    int $i = 1;
    while ($i < $exponent)
    {
        $reti = $reti * $base;
        $i++;
    }
    return $reti;
}
echo("Enter base:");
string $input = fgets(STDIN);
float $b = floatval($input);
while ($b != floatval(0))
{
    echo("Enter exponent:");
    $input = fgets(STDIN);
    int $e = intval($input);
    float $result = exp($b, $e);
    echo(strval($result));
    echo("Enter base:");
    $input = fgets(STDIN);
    $b = floatval($input);
}
?>
