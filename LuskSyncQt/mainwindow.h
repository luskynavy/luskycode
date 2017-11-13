#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QStandardItemModel>
#include "qtftp/qurlinfo.h"
#include "qtftp/qftp.h"

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = 0);
    ~MainWindow();

private slots:
    void on_PushButton_Add_clicked();

    void on_pushButton_Delete_clicked();

    void on_pushButton_Edit_clicked();

    void on_pushButton_Launch_clicked();

private:
    Ui::MainWindow *ui;

    void initTableView();
    QList<QStandardItem *> prepareRow(const QString &p1, const QString &p2, const QString &p3, const QString &p4);

    QStandardItemModel *model;

 private slots:
    void addToList(const QUrlInfo &urlInfo);
};

#endif // MAINWINDOW_H
