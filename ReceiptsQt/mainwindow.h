#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QtSql>

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

private:
    Ui::MainWindow *ui;
    bool _ready = false;
    QString _currentSort = "";
    QString _currentGroup = "";
    QString _currentName = "";
    int _currentPage = 1;
    int _currentPageSize = 10;
    int _totalPages = 1;

    QSqlDatabase _db;

    QString _host = "localhost";
    int _port = 3306;
    QString _user = "root";
    QString _password = "";
    QString _databaseName = "receipts";

    void initDb();
    void submit();
    void getGroupList();
};
#endif // MAINWINDOW_H
