#include <QString>
#include <cmath>
#include <vector>

#include "subsetSum.h"

QString SubsetSum::search(std::vector<double> products, double wantedSum)
{
    QString res = "";

    //for each 2^n possibilities
    double max = pow(2, products.size());
    for (size_t testVal = 0; testVal < max; testVal++)
    //for (int testVal = 0; testVal < Math.Pow(2, products.Length); testVal++)
    {
        double sum = 0;
        for (size_t choice = 0; choice < products.size(); choice++)
        {
            //if number is selected
            if (((1 << choice) & testVal) == (1 << choice))
            {
                sum += products[choice];
            }
        }

        //if sum is found minus epsilon
        if (fabs(sum - wantedSum) < 1e-5)
        {
            //add solution
            for (size_t j = 0; j < products.size(); j++)
            {
                //if number is selected
                if (((1 << j) & testVal) == (1 << j))
                {
                    res += QString::number(products[j]) + " ";
                }
            }
            res += "\n";
        }
    }

    return res;
}
