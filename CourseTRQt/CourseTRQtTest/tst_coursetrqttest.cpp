#include <QString>
#include <QtTest>

#include "../subsetSum.h"

class CourseTRQtTest : public QObject
{
    Q_OBJECT

public:
    CourseTRQtTest();

private Q_SLOTS:
    void testCaseNoResult();
    void testCaseResult();
};

CourseTRQtTest::CourseTRQtTest()
{
}

void CourseTRQtTest::testCaseNoResult()
{
    std::vector<double> products = { 2.41, 3.24, 2.09, 2.56, 3.28, 3.88, 1.70, 4.93, 3.30 };

    QCOMPARE(SubsetSum::search(products, 10), QString(""));
}

void CourseTRQtTest::testCaseResult()
{
    std::vector<double> products = { 2.41, 3.24, 2.09, 2.56, 3.28, 3.88, 1.70, 4.93, 3.30 };

    QCOMPARE(SubsetSum::search(products, 7.8), QString("2.41 2.09 3.3 \n"));
}


QTEST_APPLESS_MAIN(CourseTRQtTest)

#include "tst_coursetrqttest.moc"
