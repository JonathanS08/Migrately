import React, { useState } from "react";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import PasswordStrengthMeter from "./PasswordStrengthMeter";
import userService from "../../services/userService";
import migratelyLogo from "../../assets/images/migrately/migrately-logo.png";
import "./userstyles.css";
import { Formik, Form, Field, ErrorMessage } from "formik";
import {
  Card,
  Col,
  Row,
  Image,
  Button,
  OverlayTrigger,
  Tooltip,
} from "react-bootstrap";
import * as validation from "../../schemas/userSchema";
import swal from "sweetalert2";
import logger from "sabio-debug";

const _logger = logger.extend("ChangePassword");

const ChangePassword = () => {
  const [showPassword, setShowPassword] = useState(true);

  const userPasswordData = {
    password: "",
    confirmPassword: "",
  };

  const navigate = useNavigate();

  const [searchParams] = useSearchParams();

  const handleSubmit = (values) => {
    const userEmail = searchParams.get("email");
    _logger("Logger Check--->", userEmail);
    const userToken = searchParams.get("token");

    values.email = userEmail;
    values.token = userToken;
    userService
      .updatePassword(values)
      .then(onUpdateSuccess)
      .catch(onUpdateFailure);
  };

  const onUpdateSuccess = () => {
    swal
      .fire(
        "Password Changed!",
        "Your password has now been changed.",
        "success"
      )
      .then(navigate("/login"));
  };

  const onUpdateFailure = (error) => {
    _logger("password error--->", error);
    swal.fire(
      "Error",
      "Password change Failed. Please make sure your new password has a number, upper, lower and symbol and at least 8 characters.",
      "error"
    );
  };

  const onEyeClicked = () => {
    setShowPassword((prevState) => {
      return !prevState;
    });
  };

  return (
    <React.Fragment>
      <div className="row py-6 px-12 bg-info bg-gradient user-form-card-shadow">
        <Row className="align-items-center justify-content-center g-0 min-vh-100">
          <Col lg={7} md={5} className="py-8 py-xl-0">
            <Card>
              <Card.Body className="p-6">
                <div className="mb-4">
                  <Link to="/">
                    <Image
                      src={migratelyLogo}
                      className="mb-4 justify-content-center"
                      alt=""
                    />
                  </Link>
                  <h1 className="mb-1 fw-bold">Change Password</h1>
                  <span>Please enter a new password.</span>
                </div>

                <Formik
                  enableReinitialize={true}
                  initialValues={userPasswordData}
                  onSubmit={handleSubmit}
                  validationSchema={validation.validationChangePassword}
                >
                  {({ values, touched }) => (
                    <Form>
                      <div className="mb-3">
                        <ErrorMessage
                          name="password"
                          className="user-form-error"
                          component="div"
                        />
                        <label
                          htmlFor="password"
                          className="user-form-label-text "
                        >
                          Password
                        </label>

                        <Field
                          type={showPassword ? "password" : "text"}
                          className="form-control "
                          name="password"
                        />
                        {showPassword ? (
                          <span
                            className="fa fa-eye user-eye-toggle-password"
                            onClick={onEyeClicked}
                          />
                        ) : (
                          <span
                            className="fa fa-eye-slash user-eye-toggle-password"
                            onClick={onEyeClicked}
                          />
                        )}
                      </div>
                      {(touched.password || values.password !== "") && (
                        <div>
                          <span>
                            Password Strength
                            <OverlayTrigger
                              placement="bottom"
                              overlay={
                                <Tooltip id="password-Tooltip">
                                  <p>Password should have at least</p>
                                  <ul>
                                    <li>One number: 0-9</li>
                                    <li>One Upper-case: A-Z</li>
                                    <li>One Lower-case: a-z</li>
                                    <li>A symbol: !@#$%^&*</li>
                                    <li>At least 8 characters</li>
                                  </ul>
                                </Tooltip>
                              }
                            >
                              {({ ref, ...triggerHandler }) => (
                                <i
                                  ref={ref}
                                  {...triggerHandler}
                                  id="password-Tooltip"
                                  className="fas fa-question-circle ms-1"
                                ></i>
                              )}
                            </OverlayTrigger>
                          </span>
                          <PasswordStrengthMeter password={values.password} />
                        </div>
                      )}
                      <div className="mb-3">
                        <ErrorMessage
                          name="confirmPassword"
                          className="user-form-error"
                          component="div"
                        />
                        <label
                          htmlFor="confirmPassword"
                          className="user-form-label-text"
                        >
                          Confirm Password
                        </label>
                        <Field
                          type={showPassword ? "password" : "text"}
                          className="form-control"
                          name="confirmPassword"
                        />
                      </div>
                      <Col lg={12} md={12} className="mb-0 d-grid gap-2">
                        <Button variant="primary" type="submit">
                          Confirm Change
                        </Button>
                      </Col>
                    </Form>
                  )}
                </Formik>

                <hr className="my-4" />
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </div>
    </React.Fragment>
  );
};

export default ChangePassword;
//kjyjfhg
