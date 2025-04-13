#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QSqlDatabase>

QT_BEGIN_NAMESPACE
namespace Ui {
class MainWindow;
}
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void on_searchButton_clicked();

    void on_clearButton_clicked();

    void on_pagesizeComboBox_currentIndexChanged(int index);

    void on_firstPushButton_clicked();

    void on_previousPushButton_clicked();

    void on_nextPushButton_clicked();

    void on_lastPushButton_clicked();

private:
    Ui::MainWindow *ui;
    bool _ready = false;
    QString _currentSort = "";
    QString _currentGroup = "";
    QString _currentName = "";
    int _currentPage = 1;
    int _currentPageSize = 10;
    int _totalPages = 1;    

    QString _host = "localhost";
    int _port = 3306;
    QString _user = "root";
    QString _password = "";
    QString _databaseName = "receipts";

    QSqlDatabase openDb();
    void submit();
    QString buildRequestProducts();
    int countRequestProducts(QSqlDatabase db);
    QString getFilters();
    void setGroupList();
};
#endif // MAINWINDOW_H
