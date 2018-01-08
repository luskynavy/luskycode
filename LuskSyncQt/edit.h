#ifndef EDIT_H
#define EDIT_H

#include <QDialog>

namespace Ui {
class Edit;
}

class Edit : public QDialog
{
    Q_OBJECT

public:
    explicit Edit(QWidget *parent = 0);    
    ~Edit();

    void setFtpPath(const QString& text);
    QString getFtpPath() const;

    void setLocalPath(const QString& text);
    QString getLocalPath() const;

    void setName(const QString& text);
    QString getName() const;

private:
    Ui::Edit *ui;
};

#endif // EDIT_H
